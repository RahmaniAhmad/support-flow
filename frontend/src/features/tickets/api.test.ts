import { beforeEach, describe, expect, it, vi } from "vitest";
import api from "@/lib/api/axios";
import { createTicket } from "./api";

vi.mock("@/lib/api/axios", () => ({
  default: {
    post: vi.fn(),
  },
}));

describe("createTicket", () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.mocked(api.post).mockResolvedValue({
      data: { id: "ticket-1" },
    });
  });

  it("keeps the existing JSON contract when no attachments are selected", async () => {
    const request = {
      subject: "Subject",
      description: "Description",
    };

    await createTicket({ request, attachments: [] });

    expect(api.post).toHaveBeenCalledWith("/tickets", request);
  });

  it("sends ticket fields and attachments as multipart form data", async () => {
    const file = new File(["%PDF-1.7"], "report.pdf", {
      type: "application/pdf",
    });

    await createTicket({
      request: {
        subject: "Subject",
        description: "Description",
        categoryId: "category-1",
      },
      attachments: [file],
    });

    const [url, body, config] = vi.mocked(api.post).mock.calls[0];
    expect(url).toBe("/tickets");
    expect(body).toBeInstanceOf(FormData);
    expect((body as FormData).get("subject")).toBe("Subject");
    expect((body as FormData).get("description")).toBe("Description");
    expect((body as FormData).get("categoryId")).toBe("category-1");
    expect((body as FormData).getAll("attachments")).toEqual([file]);
    expect(config).toBeUndefined();
  });
});
