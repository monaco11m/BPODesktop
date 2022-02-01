create or replace function get_LabelStorageUrlsByUserAndGroupId(userId text,groupId integer)
returns table(url text,trackingNumber text,format text,shipmentGroupId bigint)
as
$$
begin
return query
select l."Url",l."TrackingNumber",l."Format",t."ShipmentGroupId"
from "AutomateLabels" u
join "Transactions" t
on u."BatchNumber"=t."ShipmentGroupId"
and u."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
and t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925' 
join "LabelStorageUrls" l 
on l."TrackingNumber"=t."TrackingNumber"
and l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'
where u."ScheduledTime">='2022-01-11'  and u."AutomateId"=55
order by u."AutomateId" asc, u."BatchNumber";
end
$$
language plpgsql

drop function get_LabelStorageUrlsByUserAndGroupId

--select get_LabelStorageUrlsByUserAndGroupId('fe69add2-2149-44dc-9415-cdd640b36925',52)
--select get_LabelStorageUrlsByUserAndGroupId('bf281bd3-c935-4b99-bb1a-a314cf341261',52)


--declare userId varchar :='fe69add2-2149-44dc-9415-cdd640b36925';
--declare groupId integer:=56;
select l."Url",l."TrackingNumber",t."TrackingNumber",t."ShipmentGroupId",l."UserId",t."UserId"
from "LabelStorageUrls" l 
join "Transactions" t
on 
l."TrackingNumber"=t."TrackingNumber"
and t."ShipmentGroupId">51 and t."ShipmentGroupId"<57
limit 100
l."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'

and t."UserId"='fe69add2-2149-44dc-9415-cdd640b36925'
and t."ShipmentGroupId"=56;
