using System.Diagnostics;

class Filosofo
{
    public int Id { get; private set; }
    public int DrinksFaltantes { get; set; }
    private List<int> GarrafasPossiveis { get; }
    private readonly Stopwatch stopwatch = new();

    public Filosofo(int id, int drinksTotal, ref int[,] matrixAdjacencia)
    {
        Id = id;
        DrinksFaltantes = drinksTotal;
        GarrafasPossiveis = PegarGarrafasPossiveis(ref matrixAdjacencia);
    }
    public void Pensando()
    {
        Console.WriteLine($"Filósofo {Id} está pensando.");

        // Simula tempo pensando, usando uma variavel Random compartilhada
        Thread.Sleep(Random.Shared.Next(500, 2000));
    }
    public void Beber(Mutex[,] garrafas)
    {
        stopwatch.Start();
        while (DrinksFaltantes > 0)
        {
            Console.WriteLine($"Filósofo {Id} está com sede.");

            int precisa = Random.Shared.Next(1, GarrafasPossiveis.Count);
            List<int> garrasfasConseguidas = [];
            Console.WriteLine($"Filósofo {Id} precisa de {precisa} garrafas");
            
            int i = 0;

            foreach (var garrafaId in GarrafasPossiveis)
            {
                if (i >= precisa) break;
                Console.WriteLine($"Filósofo {Id} está tentando pegar a garrafa [{Id},{garrafaId}].");
                if (garrafas[Id, garrafaId].WaitOne(2000))  // Tenta pegar o mutex por até 2 segundos
                {
                    Console.WriteLine($"Filósofo {Id} pegou a garrafa [{Id},{garrafaId}].");
                    garrasfasConseguidas.Add(garrafaId);  // Se conseguiu, adiciona à lista de garrafas adquiridas
                    i++;
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine($"Filósofo {Id} não conseguiu pegar a garrafa [{Id},{garrafaId}] em 2 segundos.");
                }
            }

            if (garrasfasConseguidas.Count >= precisa)
            {
                // Estado Bebendo
                Console.WriteLine($"Filósofo {Id} está bebendo das garrafas [{Id+"," + string.Join($"], [{Id},", garrasfasConseguidas)+"]"}.");
                foreach (var _ in garrasfasConseguidas)
                {
                    Thread.Sleep(1000);  // Leva 1 segundo para beber de cada garrafa
                }

                // Libera as garrafas (Mutex)
                foreach (var garrafaId in garrasfasConseguidas)
                {
                    garrafas[Id, garrafaId].ReleaseMutex();
                    Console.WriteLine($"Filósofo {Id} liberou a garrafa [{Id},{garrafaId}].");
                }

                DrinksFaltantes--;
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
                Pensando();  // Retorna para o estado Pensando
            }
            else
            {
                // Se não conseguiu pegar todas as garrafas, libera as que já pegou
                Console.WriteLine($"Filósofo {Id} não conseguiu todas as garrafas. Liberando as garrafas adquiridas.");
                foreach (var bottleId in garrasfasConseguidas)
                {
                    garrafas[Id, bottleId].ReleaseMutex();
                    Console.WriteLine($"Filósofo {Id} liberou a garrafa [{Id},{bottleId}].");
                }
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Pensando();  // Retorna para o estado Pensando antes de tentar novamente
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Filósofo {Id} terminou de beber e demorou {stopwatch.Elapsed.Seconds} segundos para finalizar.");
    }

    private List<int> PegarGarrafasPossiveis(ref int[,] matrixAdjacencia)
    {
        List<int> garrafasPossiveis = [];
        for (int i = 0; i < matrixAdjacencia.GetLength(0); i++)
        {
            if (matrixAdjacencia[Id, i] == 1)  // Verifica se existe uma aresta entre o filósofo e outro
            {
                garrafasPossiveis.Add(i);  // Adiciona à lista de garrafas disponíveis
            }
        }
        return garrafasPossiveis;
    }
}
