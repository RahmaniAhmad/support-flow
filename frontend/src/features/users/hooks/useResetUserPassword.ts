import { useMutation, useQueryClient } from "@tanstack/react-query";

import { resetUserPassword } from "../api/users";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useResetUserPassword(userId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (password: string) => resetUserPassword(userId, password),

    onSuccess() {
      queryClient.invalidateQueries({
        queryKey: queryKeys.users.detail(userId),
      });
    },
  });
}
