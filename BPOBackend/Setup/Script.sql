create or replace function get_LabelStorageUrls(id integer)
returns table(url text)
as
$$
begin
return query
select l."Url"
from "LabelStorageUrls" l 
--where l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925';
limit 1000; 
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
where u."UserName"='MSCHF'

select *
from "AutomateLabels" a
where a."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' and a."AutomateId"=1
limit 100


select count(1),t."TrackingNumber"
from "Transactions" t
--where t."ShipmentGroupId"=0
where t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'  --and t."ShipmentGroupId"=52
group by t."TrackingNumber"
limit 100



-- AutomateId = GroupId (web)
--ShipmentGroupId = BatchNumber (bd)
