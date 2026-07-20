import { AntdRegistry } from "@ant-design/nextjs-registry";
import "./globals.css";
import AppProviders from "./providers/AppProviders";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        <AppProviders>
          <AntdRegistry>{children}</AntdRegistry>
        </AppProviders>
      </body>
    </html>
  );
}
