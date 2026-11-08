import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { beforeEach, describe, expect, it, vi } from "vitest";
import TicketAttachments from "./TicketAttachments";
import { TicketAttachment } from "../types";

const mocks = vi.hoisted(() => ({
  useCurrentUser: vi.fn(),
  useTicketAttachments: vi.fn(),
  useUploadTicketAttachment: vi.fn(),
  useDeleteTicketAttachment: vi.fn(),
  useDownloadTicketAttachment: vi.fn(),
}));

vi.mock("@/features/auth/providers/CurrentUserProvider", () => ({
  useCurrentUser: mocks.useCurrentUser,
}));

vi.mock("../hooks/useTicketAttachments", () => ({
  useTicketAttachments: mocks.useTicketAttachments,
}));

vi.mock("../hooks/useUploadTicketAttachment", () => ({
  useUploadTicketAttachment: mocks.useUploadTicketAttachment,
}));

vi.mock("../hooks/useDeleteTicketAttachment", () => ({
  useDeleteTicketAttachment: mocks.useDeleteTicketAttachment,
}));

vi.mock("../hooks/useDownloadTicketAttachment", () => ({
  useDownloadTicketAttachment: mocks.useDownloadTicketAttachment,
}));

const attachment: TicketAttachment = {
  id: "attachment-1",
  fileName: "report.pdf",
  contentType: "application/pdf",
  fileSize: 1536,
  uploadedByUserId: "user-1",
  createdAtUtc: "2026-11-08T10:00:00Z",
};

const customer = {
  id: "user-1",
  name: "Customer",
  email: "customer@example.com",
  role: "Customer" as const,
  permissions: ["tickets:create", "tickets:view"],
};

describe("TicketAttachments", () => {
  const uploadMutation = vi.fn();
  const deleteMutation = vi.fn();
  const downloadMutation = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
    mocks.useCurrentUser.mockReturnValue(customer);
    mocks.useTicketAttachments.mockReturnValue({
      data: [],
      isLoading: false,
      isError: false,
    });
    mocks.useUploadTicketAttachment.mockReturnValue({
      mutate: uploadMutation,
      isPending: false,
      isError: false,
    });
    mocks.useDeleteTicketAttachment.mockReturnValue({
      mutate: deleteMutation,
      isPending: false,
      isError: false,
    });
    mocks.useDownloadTicketAttachment.mockReturnValue({
      mutate: downloadMutation,
      isPending: false,
      isError: false,
    });
  });

  it("shows loading and empty states", () => {
    mocks.useTicketAttachments.mockReturnValueOnce({
      data: undefined,
      isLoading: true,
      isError: false,
    });
    const { rerender } = render(<TicketAttachments ticketId="ticket-1" />);
    expect(document.querySelector(".ant-spin")).not.toBeNull();

    mocks.useTicketAttachments.mockReturnValue({
      data: [],
      isLoading: false,
      isError: false,
    });
    rerender(<TicketAttachments ticketId="ticket-1" />);
    expect(screen.getByText("No attachments yet.")).toBeTruthy();
  });

  it("lists file names, sizes, and download actions", async () => {
    mocks.useTicketAttachments.mockReturnValue({
      data: [attachment],
      isLoading: false,
      isError: false,
    });
    render(<TicketAttachments ticketId="ticket-1" />);

    expect(screen.getByText("report.pdf")).toBeTruthy();
    expect(screen.getByText("1.5 KB")).toBeTruthy();

    await userEvent.click(
      screen.getByRole("button", { name: "Download report.pdf" }),
    );
    expect(downloadMutation).toHaveBeenCalledWith(
      attachment.id,
      expect.any(Object),
    );
  });

  it("rejects files over the size limit", async () => {
    render(<TicketAttachments ticketId="ticket-1" />);
    const user = userEvent.setup();
    const input = document.querySelector(
      'input[type="file"]',
    ) as HTMLInputElement;
    const oversizedFile = new File(
      [new Uint8Array(10 * 1024 * 1024 + 1)],
      "large.pdf",
      { type: "application/pdf" },
    );
    await user.upload(input, oversizedFile);

    expect(screen.getByText("The file exceeds the 10 MB limit.")).toBeTruthy();
    expect(uploadMutation).not.toHaveBeenCalled();
  });

  it("uploads a selected valid file", async () => {
    render(<TicketAttachments ticketId="ticket-1" />);
    const user = userEvent.setup();
    const input = document.querySelector(
      'input[type="file"]',
    ) as HTMLInputElement;
    const validFile = new File(["%PDF-1.7"], "new.pdf", {
      type: "application/pdf",
    });
    await user.upload(input, validFile);

    expect(uploadMutation).toHaveBeenCalledWith(
      expect.objectContaining({
        ticketId: "ticket-1",
        file: validFile,
        onProgress: expect.any(Function),
      }),
      expect.objectContaining({
        onSuccess: expect.any(Function),
        onError: expect.any(Function),
      }),
    );
    expect(uploadMutation).toHaveBeenCalledTimes(1);
  });

  it("only exposes deletion to the uploader or an attachment manager", async () => {
    const user = userEvent.setup();
    mocks.useTicketAttachments.mockReturnValue({
      data: [attachment],
      isLoading: false,
      isError: false,
    });
    mocks.useCurrentUser.mockReturnValue({
      ...customer,
      id: "different-user",
      permissions: ["tickets:view"],
    });
    const { rerender } = render(
      <TicketAttachments ticketId="ticket-1" />,
    );

    expect(
      screen.queryByRole("button", { name: "Delete report.pdf" }),
    ).toBeNull();
    expect(
      screen.queryByText("Click or drag a file here to upload"),
    ).toBeNull();

    mocks.useCurrentUser.mockReturnValue({
      ...customer,
      id: "admin",
      role: "Admin",
      permissions: ["tickets:view", "tickets:manage_attachments"],
    });
    rerender(<TicketAttachments ticketId="ticket-1" />);

    const deleteButton = screen.getByRole("button", {
      name: "Delete report.pdf",
    });
    expect(deleteButton).toBeTruthy();
    await user.click(deleteButton);
    await user.click(screen.getByRole("button", { name: "OK" }));
    expect(deleteMutation).toHaveBeenCalledWith(attachment.id);
  });
});
