"use client";

import { Tag } from "antd";

import ResetPasswordButton from "./ResetPasswordButton";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { hasPermission } from "@/features/auth/authorization";
import { AppPermissions } from "@/features/auth/Permissions";
import { UserDetails } from "../types";

type Props = {
  user: UserDetails;
};

export default function UserDetailsActions({ user }: Props) {
  const currentUser = useCurrentUser();

  return (
    <div className="flex items-center gap-3">
      {hasPermission(currentUser, AppPermissions.UsersResetPassword) && (
        <ResetPasswordButton userId={user.id} />
      )}

      <Tag color={user.isActive ? "green" : "red"}>
        {user.isActive ? "Active" : "Inactive"}
      </Tag>
    </div>
  );
}
