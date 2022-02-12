create or replace function get_LabelStorageUrls(id integer)
returns table(url text)
as
$$
begin
return query
select l."Url"
from "LabelStorageUrls" l 
where l."Format"='PNG'
--where l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925';
limit 5000; 
end
$$
language plpgsql

select l."Url",l."TrackingNumber",* 
from "LabelStorageUrls" l 
where l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'
limit 100 

select count(1),l."TrackingNumber"
from "LabelStorageUrls" l 
--where l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'
group by l."TrackingNumber"

select get_LabelStorageUrls(403479)

DROP FUNCTION get_labelstorageurls(integer)

describe table "LabelStorageUrls"

select *
from "AspNetUsers" u
where u.is_active=true

select *
from "AspNetUsers" u
where --u."UserName"='MSCHF'
u."Id"='54e85c54-14e5-414c-b442-5d6c54da8d9c'

select *
from "AutomateLabels" a
where a."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' and a."AutomateId"=1
limit 100


select count(1),t."ShipmentGroupId"
from "Transactions" t
--where t."ShipmentGroupId"=0
--where t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'  --and t."ShipmentGroupId"=52
group by t."ShipmentGroupId"
limit 100

-- AutomateId = GroupId (web)
--ShipmentGroupId = BatchNumber (bd)

--31/01/2022
select *
from "AutomateLabels" u
where u."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' and u."ScheduledTime">='2022-01-11'  and u."AutomateId"=55
order by u."AutomateId" asc, u."BatchNumber";

select t."ShipmentGroupId",t."TrackingNumber",*
from "Transactions" t
where t."ShipmentGroupId"='1158' and t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'

select l."Url",l."TrackingNumber",l."Format",t."ShipmentGroupId"
from "LabelStorageUrls" l 
join "Transactions" t
on l."TrackingNumber"=t."TrackingNumber"
and l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
and t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
and t."ShipmentGroupId"=1158;

select l."Url",l."TrackingNumber",l."Format",t."ShipmentGroupId"
from "AutomateLabels" u
join "Transactions" t
on u."BatchNumber"=t."ShipmentGroupId"
and u."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
and t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
--and t."ShipmentGroupId"=1158
join "LabelStorageUrls" l 
on l."TrackingNumber"=t."TrackingNumber"
and l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'
where u."ScheduledTime">='2022-01-11'  and u."AutomateId"=55
order by u."AutomateId" asc, u."BatchNumber";


