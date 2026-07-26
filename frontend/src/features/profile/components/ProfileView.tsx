"use client";

import Link from "next/link";
import { AxiosError } from "axios";

import Button from "@/components/ui/Button";
import PageTitle from "@/components/ui/page/PageTitle";
import PageDescription from "@/components/ui/page/PageDescription";

import { useProfile } from "../hooks/useProfile";

export default function ProfileView() {
  const { data: profile, isLoading, error } = useProfile();

  if (isLoading) {
    return (
      <div className="max-w-2xl rounded-xl bg-white p-6 shadow">
        Loading profile...
      </div>
    );
  }

  const errorMessage =
    error instanceof AxiosError
      ? (error.response?.data?.message ?? "Failed to load profile.")
      : error
        ? "Failed to load profile."
        : null;

  if (errorMessage) {
    return (
      <div className="max-w-2xl rounded-xl bg-white p-6 shadow">
        <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
          {errorMessage}
        </div>
      </div>
    );
  }

  if (!profile) {
    return (
      <div className="max-w-2xl rounded-xl bg-white p-6 shadow">
        Profile not found.
      </div>
    );
  }

  return (
    <div className="max-w-2xl rounded-xl bg-white p-6 shadow">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <PageTitle>Profile</PageTitle>

          <PageDescription>View your account information.</PageDescription>
        </div>

        <Link href="/profile/edit">
          <Button>Edit Profile</Button>
        </Link>
      </div>

      <div className="flex flex-col gap-y-5">
        <ProfileItem label="Email" value={profile.email} />
        <ProfileItem label="Company" value={profile.companyName ?? ""} />
        <ProfileItem
          label="Name"
          value={`${profile.firstName} ${profile.lastName}`}
        />

        <ProfileItem label="Phone" value={profile.phone ?? "-"} />

        <ProfileItem label="Role" value={profile.role} />
      </div>
    </div>
  );
}

function ProfileItem({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <div className="text-sm text-slate-500">{label}</div>

      <div className="font-medium text-slate-800">{value}</div>
    </div>
  );
}
