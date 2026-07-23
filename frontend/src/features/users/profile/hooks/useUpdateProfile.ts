import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateProfile } from "../services/profile.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useUpdateProfile() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateProfile,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.users.profile(),
      });
    },
  });
}
