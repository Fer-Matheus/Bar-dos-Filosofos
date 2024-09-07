int[,] matrixAdjacencia = 
{
        { 0, 1, 0, 0, 1 },
        { 1, 0, 1, 0, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 1 },
        { 1, 0, 0, 1, 0 }
};

int[,] matrixAdjacencia2 = 
{
{0, 1, 0, 0, 0, 1,},
{1, 0, 1, 0, 0, 0,},
{0, 1, 0, 1, 0, 1,},
{0, 0, 1, 0, 1, 1,},
{0, 0, 0, 1, 0, 1,},
{1, 0, 1, 1, 1, 0,}
};

int[,] matrixAdjacencia3 = 
{
    { 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0},
    { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
    { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    { 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0},
    { 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0},
    { 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0},
    { 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 0},
    { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0},
    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1},
    { 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 1},
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1},
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0}
};

List<(int, int[,])> datas = [
    (6, matrixAdjacencia),
    (6, matrixAdjacencia2),
    (3, matrixAdjacencia3),
];

var data = datas[2];
int qtdeBeber = data.Item1;
int[,] matrix = data.Item2;

// Estoque guarda as garrafas disponiveis
Estoque estoque = new(matrix);

// Preparando os filosofos para serem executados em paralelo
List<Task> filosofos = [];
for (int i = 0; i < matrix.GetLength(0); i++)
{
    var filosofo = new Filosofo(i, qtdeBeber, ref matrix);
    filosofos.Add(Task.Run(() => filosofo.Beber(estoque.Garrafas)));
}

Console.WriteLine("Todos já foram preparados e começarão agora...");

Task.WaitAll([.. filosofos]);

Console.WriteLine("Todos os filósofos terminaram de beber.");