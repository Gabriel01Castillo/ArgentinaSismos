CREATE FUNCTION [dbo].[GetDistanceBetween]
(
    @Lat1 float,
    @Long1 float,
    @Lat2 float,
    @Long2 float
)
RETURNS float
AS
BEGIN

    DECLARE @RetVal float;
    SET @RetVal = ( SELECT geography::Point(@Lat1, @Long1, 4326).STDistance(geography::Point(@Lat2, @Long2, 4326)) / 1609.344 );

RETURN @RetVal;

END


--DECLARE @StartingLatitude FLOAT, @StartingLongitude FLOAT;
--DECLARE @MaxDistance FLOAT = 50;

--SELECT * FROM Earthquake 
--WHERE dbo.GetDistanceBetween(-32.7161111111, -70.1450000000, latitude, longitude) <= @MaxDistance;