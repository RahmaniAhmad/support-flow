import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createUser } from "../api/users";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useCreateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createUser,

    onSuccess() {
      queryClient.invalidateQueries({
        queryKey: queryKeys.users.all,
      });
    },
  });
}
