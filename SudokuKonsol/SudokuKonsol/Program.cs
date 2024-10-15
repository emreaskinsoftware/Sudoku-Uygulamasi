using System;
using System.Linq;

namespace SudokuKonsol
{
    class Program
    {
        static Random rnd = new Random(); // Global Random nesnesi
        static int[,] çözümTahtası = new int[9, 9]; // Tam çözüm

        static void Main(string[] args)
        {
            int[,] oyunTahtası = new int[9, 9];

            // Backtracking ile Sudoku tahtasını doldur
            if (TahtayıDoldur(çözümTahtası))
            {
                // Çözümün bir kopyasını oluştur (boşluklar olmadan)
                Array.Copy(çözümTahtası, oyunTahtası, çözümTahtası.Length);

                // Rastgele 40 hücreyi boş bırak (0 yap)
                HücreleriBoşalt(oyunTahtası, 40);

                Console.WriteLine("Sudoku Tahtası Hazır:");
                Yazdir(oyunTahtası);

                // Kullanıcıya oyun oynat
                OyunOyna(oyunTahtası);
            }
            else
            {
                Console.WriteLine("Sudoku tahtası doldurulamadı.");
            }

            Console.ReadLine();
        }

        // Sudoku tahtasını backtracking ile rastgele doldurur
        public static bool TahtayıDoldur(int[,] oyunTahtası)
        {
            return TahtayıDoldurHelper(oyunTahtası, 0, 0);
        }

        // Recursive backtracking fonksiyonu
        public static bool TahtayıDoldurHelper(int[,] oyunTahtası, int satır, int sutun)
        {
            if (satır == 9)
                return true;

            if (sutun == 9)
                return TahtayıDoldurHelper(oyunTahtası, satır + 1, 0);

            int[] sayilar = DiziKaristir(Enumerable.Range(1, 9).ToArray());

            foreach (int sayı in sayilar)
            {
                if (Kontrol(oyunTahtası, satır, sutun, sayı))
                {
                    oyunTahtası[satır, sutun] = sayı;

                    if (TahtayıDoldurHelper(oyunTahtası, satır, sutun + 1))
                    {
                        return true;
                    }

                    oyunTahtası[satır, sutun] = 0;
                }
            }

            return false;
        }

        // Sayıların sırasını karıştırmak için bir yardımcı fonksiyon
        public static int[] DiziKaristir(int[] dizi)
        {
            for (int i = dizi.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(0, i + 1);
                int temp = dizi[i];
                dizi[i] = dizi[j];
                dizi[j] = temp;
            }
            return dizi;
        }

        // Sudoku kurallarına uygunluğu kontrol eder
        public static bool Kontrol(int[,] oyunTahtası, int satır, int sutun, int sayı)
        {
            for (int i = 0; i < 9; i++)
            {
                if (oyunTahtası[satır, i] == sayı || oyunTahtası[i, sutun] == sayı)
                {
                    return false;
                }
            }

            int BaslangicSatir = satır - satır % 3;
            int BaslangicSutun = sutun - sutun % 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (oyunTahtası[i + BaslangicSatir, j + BaslangicSutun] == sayı)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Sudoku tahtasında rastgele 40 hücreyi boşalt (0 yap)
        public static void HücreleriBoşalt(int[,] oyunTahtası, int boşSayısı)
        {
            int boşalan = 0;
            while (boşalan < boşSayısı)
            {
                int satır = rnd.Next(0, 9);
                int sutun = rnd.Next(0, 9);

                if (oyunTahtası[satır, sutun] != 0)
                {
                    oyunTahtası[satır, sutun] = 0;
                    boşalan++;
                }
            }
        }

        // Kullanıcının oyunu oynamasını sağla
        public static void OyunOyna(int[,] oyunTahtası)
        {
            int yanlışHak = 3;

            while (yanlışHak > 0)
            {
                Console.WriteLine("Bir satır (1-9) seçin:");
                int satır = int.Parse(Console.ReadLine()) - 1;

                Console.WriteLine("Bir sütun (1-9) seçin:");
                int sutun = int.Parse(Console.ReadLine()) - 1;

                Console.WriteLine("Bu hücreye hangi sayıyı yerleştirmek istersiniz? (1-9):");
                int sayı = int.Parse(Console.ReadLine());

                if (oyunTahtası[satır, sutun] != 0)
                {
                    Console.WriteLine("Bu hücre zaten doldurulmuş.");
                    continue;
                }

                if (sayı == çözümTahtası[satır, sutun])
                {
                    oyunTahtası[satır, sutun] = sayı;
                    Console.WriteLine("Doğru!");
                }
                else
                {
                    yanlışHak--;
                    Console.WriteLine($"Yanlış! Kalan yanlış hakkınız: {yanlışHak}");
                }

                Yazdir(oyunTahtası);

                if (TahtaTamamlandiMi(oyunTahtası))
                {
                    Console.WriteLine("Tebrikler! Sudoku tahtasını başarıyla doldurdunuz.");
                    Console.WriteLine("Çözümü görmek ister misiniz? (E/H)");
                    string cevap = Console.ReadLine();
                    if (cevap.ToUpper() == "E")
                    {
                        Console.WriteLine("İşte çözüm:");
                        Yazdir(çözümTahtası);
                    }
                    return;
                }
            }

            Console.WriteLine("Üzgünüz, 3 yanlış hakkınızı doldurdunuz. Oyun bitti.");
            Console.WriteLine("İşte çözüm:");
            Yazdir(çözümTahtası); // Oyun bittiğinde çözümü yazdır
        }

        // Sudoku tahtasının tamamlanıp tamamlanmadığını kontrol eder
        public static bool TahtaTamamlandiMi(int[,] oyunTahtası)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (oyunTahtası[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Sudoku tahtasını ekrana yazdırma fonksiyonu
        public static void Yazdir(int[,] oyunTahtası)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(oyunTahtası[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
