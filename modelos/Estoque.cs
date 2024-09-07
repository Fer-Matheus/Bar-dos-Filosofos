class Estoque
{
    public Mutex[,] Garrafas { get; set; }

    public Estoque(int[,] matrixAdjacencia)
    {
        int qtdeFilosofos = matrixAdjacencia.GetLength(0);
        Garrafas = new Mutex[qtdeFilosofos, qtdeFilosofos];
        for (int i = 0; i < qtdeFilosofos; i++) 
        {
            for (int j = 0; j < qtdeFilosofos; j++)
            {
                if ((matrixAdjacencia[i, j] is 1) && (Garrafas[i,j] is null))
                {
                    var temp = new Mutex();
                    Garrafas[i,j] = temp;
                    Garrafas[j,i] = temp;
                }
            }
        }
    }
}