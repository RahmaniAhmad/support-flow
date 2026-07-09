import { UserProvider } from "@/features/auth/providers/UserProvider";
import AppLayout from "@/components/layout/AppLayout";
import { redirect } from "next/navigation";
import { getCurrentUser } from "@/features/auth/server/getCurrentUser";

export default async function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const user = await getCurrentUser();

  if (!user) {
    redirect("/login");
  }

  return (
    <UserProvider user={user}>
      <AppLayout>{children}</AppLayout>;
    </UserProvider>
  );
}
