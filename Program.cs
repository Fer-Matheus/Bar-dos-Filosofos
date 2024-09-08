var data = Matriz.ParseFileToMatriz("grafos/" + args[0] + ".txt");
var qtdeBeber = data.Item1;
var matriz = data.Item2;

// Estoque guarda as garrafas disponiveis
Estoque estoque = new(matriz);

// Preparando os filosofos para serem executados em paralelo
List<Task> filosofos = [];
for (int i = 0; i < matriz.GetLength(0); i++)
{
    var filosofo = new Filosofo(i, qtdeBeber, ref matriz);
    filosofos.Add(Task.Run(() => filosofo.Beber(estoque.Garrafas)));
}

Console.WriteLine("Todos já foram preparados e começarão agora...");

Task.WaitAll([.. filosofos]);

Console.WriteLine("Todos os filósofos terminaram de beber.");