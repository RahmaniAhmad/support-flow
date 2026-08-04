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

      <div className="flex flex-col gap-y-5 p-4 border border-gray-300 rounded-xl">
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
