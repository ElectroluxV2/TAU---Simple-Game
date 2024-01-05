using FluentAssertions;

namespace Tests;

/**
 * Hence this generator is used within Unity Game Engine,
 * it makes absolutely no sense to test Unity features such
 * as physics capabilities as they are already well tested.
 */
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

    [Fact]
    public void MapGenerator_ShouldGenerateStart()
    {
        var sut = new MapGenerator();
        var map = sut.Generate(50, 50);

        map.Enumerate().Select(x => x.cell).Should().ContainSingle(x => x.IsStartCell);
    }
    
    [Fact]
    public void MapGenerator_ShouldGenerateStop()
    {
        var sut = new MapGenerator();
        var map = sut.Generate(50, 50);

        map.Enumerate().Select(x => x.cell).Should().ContainSingle(x => x.IsStopCell);
    }

    [Fact]
    public void MapGenerator_ShouldGenerateStartAndStop_WithinDifferentCoords()
    {
        var sut = new MapGenerator();
        var map = sut.Generate(50, 50);

        var (startX, startY, _) = map.Enumerate().First(x => x.cell.IsStartCell);
        var (stopX, stopY, _) = map.Enumerate().First(x => x.cell.IsStopCell);

        startX.Should().NotBe(stopX);
        startY.Should().NotBe(stopY);
    }

    [Fact]
    public void MapGenerator_ShouldGenerateRandomObstacles_NotWithinWalls()
    {
        var sut = new MapGenerator();
        var map = sut.Generate(50, 50);

        var obstacles = map
            .Enumerate()
            .Where(f => f.x != 0 && f.x != 49 && f.y != 0 && f.y != 49)
            .Select(x => x.cell);

        obstacles
            .Count()
            .Should()
            .BeGreaterThan(1, "Generated map should contain more than 1 obstacle that is not wall");
    }
}