import type { Metadata } from 'next';
import './globals.css';
import Navbar from '@/components/layout/Navbar';
import Footer from '@/components/layout/Footer';

export const metadata: Metadata = {
  title: 'ArtVerse - Nền Tảng Triển Lãm & Đánh Giá Mỹ Thuật Cao Cấp',
  description: 'Hệ thống quản lý triển lãm, chấm thi và danh mục tác phẩm mỹ thuật học đường Clean Architecture .NET 8 & Next.js 14',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="vi" className="dark">
      <body className="bg-[#090a0f] text-slate-100 min-h-screen flex flex-col antialiased">
        <Navbar />
        <main className="flex-1">
          {children}
        </main>
        <Footer />
      </body>
    </html>
  );
}