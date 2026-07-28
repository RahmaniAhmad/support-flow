import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateUser } from "../api/users";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { UpdateUserRequest } from "../types";

export function useUpdateUser(userId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateUserRequest) => updateUser(userId, data),

    onSuccess() {
      queryClient.invalidateQueries({
        queryKey: queryKeys.users.detail(userId),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.users.lists(),
      });
    },
  });
}
