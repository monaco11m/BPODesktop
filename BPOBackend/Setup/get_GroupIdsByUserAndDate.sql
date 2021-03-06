create or replace function get_GroupIdsByUserAndDate(userId text,startDate date,endDate date)
returns table(AutomateId bigint)
as
$$
begin
return query
select distinct (a."AutomateId") as AutomateId
from "AutomateLabels" a
where a."UserId"=userId and a."CreatedDate" >= startDate
order by AutomateId;
end
$$
language plpgsql

--select get_GroupIdsByUserAndDate('fe69add2-2149-44dc-9415-cdd640b36925','2022-01-01','2022-01-31')

