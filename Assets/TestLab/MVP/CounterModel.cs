public class CounterModel
{
    private int _count;
    public int Count => _count;

    public CounterModel(int count)
    {
        _count = count;
    }

    public void CountUp()
    {
        _count++;
    }
}
