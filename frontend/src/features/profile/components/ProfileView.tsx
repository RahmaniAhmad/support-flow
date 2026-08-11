"use client";

import Link from "next/link";

import Button from "@/components/ui/Button";
import PageTitle from "@/components/ui/page/PageTitle";
import PageDescription from "@/components/ui/page/PageDescription";

import { UserProfile } from "../types";

type Props = {
  profile: UserProfile;
};

export default function ProfileView({ profile }: Props) {
  const fullName = `${profile.firstName} ${profile.lastName}`.trim();

  return (
    <div className="rounded-xl bg-white p-6 shadow">
      <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <PageTitle>Profile</PageTitle>
          <PageDescription>View your account information.</PageDescription>
        </div>

        <Link href="/profile/edit">
          <Button>Edit Profile</Button>
        </Link>
      </div>

      <dl className="divide-y divide-gray-200 rounded-xl border border-gray-200">
        <ProfileItem label="Email" value={profile.email} />
        <ProfileItem label="Company" value={profile.companyName ?? "-"} />
        <ProfileItem label="Name" value={fullName || "-"} />
        <ProfileItem label="Phone" value={profile.phone ?? "-"} />
        <ProfileItem label="Role" value={profile.role} />
      </dl>
    </div>
  );
}

function ProfileItem({ label, value }: { label: string; value: string }) {
  return (
    <div className="grid gap-1 px-4 py-4 sm:grid-cols-3 sm:gap-4">
      <dt className="text-sm font-medium text-slate-500">{label}</dt>

      <dd className="text-sm font-medium text-slate-800 sm:col-span-2">
        {value}
      </dd>
    </div>
  );
}
