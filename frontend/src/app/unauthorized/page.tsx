import BackButton from "@/components/ui/navigation/BackButton";
import { getCurrentUser } from "@/features/auth/server/getCurrentUser";
import { getHomeRoute } from "@/features/auth/utils/getHomeRoute";

export default async function UnauthorizedPage() {
  const currentUser = await getCurrentUser();

  const href = currentUser ? getHomeRoute(currentUser.permissions) : "/login";

  return (
    <div className="flex min-h-100 flex-col items-center justify-center text-center">
      <h1 className="mb-3 text-3xl font-bold text-slate-800">Access Denied</h1>

      <p className="mb-6 text-slate-500">
        You do not have permission to access this page.
      </p>
      <BackButton fallbackHref={href} label="Go Back" />
    </div>
  );
}
