import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import {
  activateTicketCategory,
  assignTicketCategory,
  createTicketCategory,
  deactivateTicketCategory,
  getActiveTicketCategories,
  getActiveTicketCategoriesForTicket,
  getTicketCategories,
  removeTicketCategory,
  updateTicketCategory,
} from "../api/ticketCategories";
import {
  TicketCategoryFilters,
  TicketCategoryRequest,
} from "../types";

export function useTicketCategories(filters: TicketCategoryFilters) {
  return useQuery({
    queryKey: queryKeys.ticketCategories.list(filters),
    queryFn: () => getTicketCategories(filters),
  });
}

export function useActiveTicketCategories(enabled = true) {
  return useQuery({
    queryKey: queryKeys.ticketCategories.active(),
    queryFn: getActiveTicketCategories,
    enabled,
  });
}

export function useActiveTicketCategoriesForTicket(
  ticketId: string,
  enabled = true,
) {
  return useQuery({
    queryKey: queryKeys.ticketCategories.activeForTicket(ticketId),
    queryFn: () => getActiveTicketCategoriesForTicket(ticketId),
    enabled: enabled && !!ticketId,
  });
}

export function useCreateTicketCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: createTicketCategory,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: queryKeys.ticketCategories.all,
      });
    },
  });
}

export function useUpdateTicketCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({
      id,
      request,
    }: {
      id: string;
      request: TicketCategoryRequest;
    }) => updateTicketCategory(id, request),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: queryKeys.ticketCategories.all,
      });
      await queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.all,
      });
    },
  });
}

export function useSetTicketCategoryStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({
      id,
      isActive,
    }: {
      id: string;
      isActive: boolean;
    }) =>
      isActive
        ? activateTicketCategory(id)
        : deactivateTicketCategory(id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: queryKeys.ticketCategories.all,
      });
      await queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.all,
      });
    },
  });
}

export function useAssignTicketCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({
      ticketId,
      categoryId,
    }: {
      ticketId: string;
      categoryId: string;
    }) => assignTicketCategory(ticketId, categoryId),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.all,
      });
    },
  });
}

export function useRemoveTicketCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: removeTicketCategory,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.all,
      });
    },
  });
}
