using System;
using System.Collections;
using System.Collections.Generic;

public class CartesianMap<T> : IEnumerable<T>
{
    private readonly int _sizeX, _sizeY;
    private T[] _buffer;

    public CartesianMap(int sizeX, int sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _buffer = new T[sizeX * sizeY];
    }

    public void Instantiate(Func<int, int, T> initiator)
    {
        for (var x = 0; x < _sizeX; x++)
        {
            for (var y = 0; y < _sizeY; y++)
            {
                this[x, y] = initiator(x, y);
            }
        }
    }
    
    public T this[int x, int y]
    {
        get => _buffer[pair(x, y)];
        set => _buffer[pair(x, y)] = value;
    }

    public T this[int i]
    {
        get => _buffer[i];
        set => _buffer[i] = value;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < _sizeX * _sizeY; i++)
        {
            yield return this[i];
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> GetReverseEnumerator()
    {
        for (var i = _sizeX * _sizeY - 1; i >= 0; i--)
        {
            yield return this[i];
        }
    }

    public IEnumerable<(int x, int y, T cell)> Enumerate()
    {
        for (var i = 0; i < _sizeX * _sizeY; i++)
        {
            var (x, y) = unpair(i);
            yield return (x, y, this[i]);
        }
    }

    private static int pair(int x, int y) {
        return x >= y ? x * x + x + y : y * y + x;
    }

    private static (int, int) unpair(int z) {
        var b = (int) Math.Sqrt(z);
        var a = z - b * b;

        return a < b ? (a, b) : (b, a - b);  
    }
}
