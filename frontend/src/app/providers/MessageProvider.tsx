"use client";

import { createContext, useContext } from "react";
import { message } from "antd";

const MessageContext = createContext<
  ReturnType<typeof message.useMessage>[0] | null
>(null);

export function useMessage() {
  const context = useContext(MessageContext);

  if (!context) {
    throw new Error("useMessage must be used inside MessageProvider");
  }

  return context;
}

export default function MessageProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [messageApi, contextHolder] = message.useMessage();

  return (
    <>
      {contextHolder}

      <MessageContext.Provider value={messageApi}>
        {children}
      </MessageContext.Provider>
    </>
  );
}
