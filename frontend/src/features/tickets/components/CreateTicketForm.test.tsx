import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { beforeEach, describe, expect, it, vi } from "vitest";
import CreateTicketForm from "./CreateTicketForm";

const mocks = vi.hoisted(() => ({
  mutate: vi.fn(),
  push: vi.fn(),
  success: vi.fn(),
}));

vi.mock("next/navigation", () => ({
  useRouter: () => ({ push: mocks.push }),
}));

vi.mock("@/app/providers/MessageProvider", () => ({
  useMessage: () => ({ success: mocks.success }),
}));

vi.mock("../hooks/useCreateTicket", () => ({
  useCreateTicket: () => ({
    mutate: mocks.mutate,
    isPending: false,
    error: null,
  }),
}));

vi.mock("@/features/ticket-categories/hooks/useTicketCategories", () => ({
  useActiveTicketCategories: () => ({
    data: [],
    isLoading: false,
    error: null,
  }),
}));

describe("CreateTicketForm attachments", () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mocks.mutate.mockImplementation((_request, options) => {
      options.onSuccess({ id: "created-ticket" });
    });
  });

  function fillTicketDetails() {
    fireEvent.change(screen.getByPlaceholderText("Unable to log in"), {
      target: { value: "Subject" },
    });
    fireEvent.change(screen.getByPlaceholderText("Describe your issue..."), {
      target: { value: "Description" },
    });
  }

  function fileInput() {
    return document.querySelector('input[type="file"]') as HTMLInputElement;
  }

  it("submits ticket details without files using the JSON-compatible input", async () => {
    render(<CreateTicketForm />);
    fillTicketDetails();
    fireEvent.submit(document.querySelector("form")!);

    await waitFor(() => expect(mocks.mutate).toHaveBeenCalled());
    expect(mocks.mutate).toHaveBeenCalledWith(
      {
        request: {
          subject: "Subject",
          description: "Description",
          categoryId: undefined,
        },
        attachments: [],
      },
      expect.objectContaining({ onSuccess: expect.any(Function) }),
    );
    expect(mocks.push).toHaveBeenCalledWith("/tickets/created-ticket");
  });

  it("submits multiple selected files and allows removing one before submit", async () => {
    const user = userEvent.setup();
    render(<CreateTicketForm />);
    const first = new File(["%PDF-1.7 one"], "first.pdf", {
      type: "application/pdf",
    });
    const second = new File(["%PDF-1.7 two"], "second.pdf", {
      type: "application/pdf",
    });
    await user.upload(fileInput(), [first, second]);

    expect(screen.getByText("first.pdf")).toBeTruthy();
    expect(screen.getByText("second.pdf")).toBeTruthy();
    expect(screen.getAllByText("12 B")).toHaveLength(2);

    await user.click(screen.getByRole("button", { name: "Remove first.pdf" }));
    fillTicketDetails();
    fireEvent.submit(document.querySelector("form")!);

    await waitFor(() => expect(mocks.mutate).toHaveBeenCalled());
    expect(mocks.mutate.mock.calls[0][0].attachments).toEqual([second]);
  }, 15000);

  it("rejects unsupported, oversized, and sixth files", async () => {
    const user = userEvent.setup();
    render(<CreateTicketForm />);

    fireEvent.change(fileInput(), {
      target: {
        files: [
          new File(["bad"], "script.exe", {
            type: "application/octet-stream",
          }),
        ],
      },
    });
    expect(
      screen.getByText("script.exe has an unsupported file extension."),
    ).toBeTruthy();
    expect(screen.queryByText("script.exe")).toBeNull();

    await user.upload(
      fileInput(),
      new File([new Uint8Array(10 * 1024 * 1024 + 1)], "large.pdf", {
        type: "application/pdf",
      }),
    );
    expect(screen.getByText("large.pdf exceeds the 10 MB limit.")).toBeTruthy();

    const files = Array.from(
      { length: 6 },
      (_, index) =>
        new File([`%PDF-1.7 ${index}`], `file-${index}.pdf`, {
          type: "application/pdf",
        }),
    );
    await user.upload(fileInput(), files);

    expect(
      screen.getByText("A ticket can have at most 5 attachments."),
    ).toBeTruthy();
    expect(screen.getByText("file-4.pdf")).toBeTruthy();
    expect(screen.queryByText("file-5.pdf")).toBeNull();
  });
});
