create or replace function get_LabelStorageUrlsByUserAndGroupId(userId text,groupId integer,startDate date)
returns table(url text,trackingNumber text,format text,batchNumber bigint,itemQuantity integer,ItemSku text)
as
$$
begin
return query
select l."Url",l."TrackingNumber",l."Format",u."BatchNumber",u."ItemQuantity",u."ItemSku"
from "AutomateLabels" u
join "Transactions" t
on u."BatchNumber"=t."ShipmentGroupId"
and u."UserId"=userId--'fe69add2-2149-44dc-9415-cdd640b36925' 
and t."UserId"=userId--'fe69add2-2149-44dc-9415-cdd640b36925' 
join "LabelStorageUrls" l 
on l."TrackingNumber"=t."TrackingNumber"
and l."UserId"=userId--'fe69add2-2149-44dc-9415-cdd640b36925'
where u."ScheduledTime">=startDate--'2022-01-11'  
and u."AutomateId"=groupId--55
order by u."AutomateId" asc, u."BatchNumber"
--limit 100
;
end
$$
language plpgsql

--drop function get_LabelStorageUrlsByUserAndGroupId

--select get_LabelStorageUrlsByUserAndGroupId('fe69add2-2149-44dc-9415-cdd640b36925',55,'2022-01-11')

--select l."Url",l."TrackingNumber",l."Format",u."BatchNumber"
--from "AutomateLabels" u
--join "Transactions" t
--on u."BatchNumber"=t."ShipmentGroupId"
--and u."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
--and t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
--join "LabelStorageUrls" l 
--on l."TrackingNumber"=t."TrackingNumber"
--and l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'
--where u."ScheduledTime">='2022-01-11'  
--and u."AutomateId"=55 and u."BatchNumber"=1173
--order by u."AutomateId" asc, u."BatchNumber";
