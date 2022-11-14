using WMS.Exceptions;
using WMS.Models.Dtos.Layout;

namespace WMS.Converters
{
    public static class PositionConverter
    {
        public static Coordinates ConvertToStruct(this string position)
        {
            if (position is null)
                throw new NotFoundException("position Not Found");

            var splitted = position.Split('.');

            int x = 0;
            int y = 0;
            int z = 0;

            bool successfullyParsed = true;

            successfullyParsed = successfullyParsed & int.TryParse(splitted[0], out x);
            successfullyParsed = successfullyParsed & int.TryParse(splitted[1], out y);
            successfullyParsed = successfullyParsed & int.TryParse(splitted[2], out z);

            if (!successfullyParsed)
                throw new NotFoundException("Cannot convert position");

            var result = new Coordinates(x, y, z);

            return result;
        }
    }
}
