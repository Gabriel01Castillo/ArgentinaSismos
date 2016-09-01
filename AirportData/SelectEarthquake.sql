select e.UTCDateTime,e.Latitude,e.Longitude,e.Depth,e.Magnitude,e.IsSensible,e.wasnotified,p.PlaceName,p.Country , e.PublicatedDatTime ,DATEDIFF(MINUTE, e.UTCDateTime, e.PublicatedDatTime) as Retardo_en_aviso_Min , e.Source_Id   from Earthquake  as  e JOIN Place as p
ON ( e.Place_Id= p.Id)order by UTCDateTime desc

select count(p.Id) FROM Earthquake  as  e JOIN Place as p
ON ( e.Place_Id= p.Id);

select * from mylogs order by date desc

select * from tweet order by datetime desc
--delete from MyLogs

--
 -- declare @num_hours int; 
 -- set @num_hours = 4;   
 -- update Earthquake set wasnotified = 0,[IsSensible]=1 , utcdatetime=  dateadd(HOUR, @num_hours, getdate()) where id ='B3DEDD40-CF60-4F7F-AF4D-258E9336C0D2';

--insert into registrationdevice values(getdate(),'359718776788248','APA91bFdNCI4oW5Cdg_CuxS-7H-X80reSTdUXX5v3svXTHoPzmcm01o3XzqfdnkVlpGUKMnySDhWya2_t3x-h1RUg890NQbsba_TufdlwtK8SrLcR4lBicOM7RG37zAeBtxEiDD1QggJMuiYk0C4vQ0QtyYI7KKvtg')
