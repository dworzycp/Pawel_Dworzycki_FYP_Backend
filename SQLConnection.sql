SELECT *
FROM GPS_Coords 
WHERE user_id = 1 AND clusterId IS NULL

-- SELECT latitude, longitude
-- FROM GPS_Coords 
-- WHERE user_id = 1

-- ALTER TABLE GPS_Coords ADD clusterId INT NULL