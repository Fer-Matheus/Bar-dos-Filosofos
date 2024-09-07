static class Matriz
{
    public static (int, int[,]) ParseFileToMatriz(string path)
    {
        var lines = File.ReadAllLines(path);

        int qtdeFilosofos = lines.Length;

        int[,] matriz = new int[qtdeFilosofos, qtdeFilosofos];

        int qtdeBeber = qtdeFilosofos <= 6 ? 6 : 3;

        {
            int i = 0;
            foreach (var line in lines)
            {
                int j = 0;
                foreach (var s in line.Trim().Split(','))
                {
                    matriz[i, j] = int.Parse(s);
                    j++;
                }
                i++;
            }
        }
        return (qtdeBeber, matriz);
    }
}