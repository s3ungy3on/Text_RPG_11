namespace Text_RPG_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SafeResize(200, 50);
            Console.OutputEncoding = global::System.Text.Encoding.UTF8;

            GameManager gameManager = new GameManager();
            
            gameManager.GameStart();
        }

        static void SafeResize(int cols, int rows)
        {
            try
            {
                cols = Math.Max(1, cols);
                rows = Math.Max(1, rows);

                if (Console.LargestWindowWidth > 0) cols = Math.Min(cols, Console.LargestWindowWidth);
                if (Console.LargestWindowHeight > 0) rows = Math.Min(rows, Console.LargestWindowHeight);
                int bufW = Math.Max(Console.BufferWidth, cols);
                int bufH = Math.Max(Console.BufferHeight, rows);
                if (bufW != Console.BufferWidth || bufH != Console.BufferHeight)
                    Console.SetBufferSize(bufW, bufH);
                Console.SetWindowSize(cols, rows);
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is PlatformNotSupportedException ||
                ex is ArgumentOutOfRangeException)
            {
                Console.WriteLine($"콘솔 크기 변경 실패 {ex.GetType().Name} 발생");
            }
        }
    }
}
