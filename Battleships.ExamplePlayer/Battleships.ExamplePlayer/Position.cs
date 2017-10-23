namespace Battleships.ExamplePlayer
{
    public class Position
    {
        public Position(char row, int col)
        {
            Col = col;
            Row = row;
        }
        public int Col { get; set; }
        public char Row { get; set; }
    }
}