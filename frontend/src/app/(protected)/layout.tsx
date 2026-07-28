import { CurrentUserProvider } from "@/features/auth/providers/CurrentUserProvider";
import AppLayout from "@/components/layout/AppLayout";
import { redirect } from "next/navigation";
import { getCurrentUser } from "@/features/auth/server/getCurrentUser";

export default async function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const currentUser = await getCurrentUser();

  if (!currentUser) {
    redirect("/login");
  }

  return (
    <CurrentUserProvider currentUser={currentUser}>
      <AppLayout>{children}</AppLayout>;
    </CurrentUserProvider>
  );
}
