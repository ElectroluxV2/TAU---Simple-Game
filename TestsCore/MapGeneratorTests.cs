using FluentAssertions;

namespace TestsCore;

public class MapGeneratorTests
{
        
    [Fact]
    public void MapGenerator_ShouldGenerateWalls()
    {
        var sut = new MapGenerator();
        var map = sut.Generate(50, 50);
            
        for (var x = 0; x < 50; x++)
        {
            for (var y = 0; y < 50; y++)
            {
                if (x == 0 || x == 49 || y == 0 || y == 49)
                    map[x, y].IsWall.Should().BeTrue();
            }
        }
    }
}