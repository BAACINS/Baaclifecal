select  *

into dbo.fundAmt from (
select gender,age,benefitCode,savAmt,fundAmt,savAmt*8 as reward
,savAmt*120  as netMonth 
,savAmt*40*3  as netQuarter 
,(savAmt*6*0.04) as discountSemiAnnual
,(savAmt*20*6)*(1-0.04) as netSemiAnnual
,(savAmt*12*0.04) as discountAnnual
,(savAmt*10*12)*(1-0.04)  as netAnnual
from dbo.fundAmt_f

union

select gender,age,benefitCode,savAmt,fundAmt,savAmt*8 as reward
,savAmt*120  as netMonth 
,savAmt*40*3  as netQuarter 
,(savAmt*6*0.04) as discountSemiAnnual
,(savAmt*20*6)*(1-0.04) as netSemiAnnual
,(savAmt*12*0.04) as discountAnnual
,(savAmt*10*12)*(1-0.04)  as netAnnual
from dbo.fundAmt_m) a 