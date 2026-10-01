import { getCurrentUser } from "@/features/auth/server/getCurrentUser";
import { redirect } from "next/navigation";

export default async function HomePage() {
  const currentUser = await getCurrentUser();
  if (!currentUser) {
    redirect("/login");
  }

  if (currentUser.role === "Customer") {
    redirect("/tickets");
  }

  redirect("/dashboard");
}
