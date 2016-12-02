select *
from (SELECT f.AGE,f.GENDER,f.SAVAMT,f.BENEFITCODE,
			'รายเดือน' AS PAYTYPE,
			'120' AS INSTALMENT,
			SAVAMT AS INSTALMENTAMT,
			'-' AS DISCOUNT,
			SAVAMT AS TOTINSTALMENTAMT,
			NETMONTH AS CONTACTAMT,
			FUNDAMT + REWARD AS TOTCONTACTAMT,
			(FUNDAMT + REWARD) - NETMONTH AS NETGAIN FROM DBO.FUNDAMT f 


union

select age,gender,savAmt,benefitCode,
'ราย 3 เดือน' AS paytype,
'40' AS instalment,
savAmt*3 as instalmentAmt,
'-' as discount,
savAmt*3 as totInstalmentAmt,
netQuarter as contactAmt,
fundAmt + reward as totContactAmt,
(fundAmt + reward) - netQuarter as netGain from dbo.fundAmt 
    
union

select age,gender,savAmt,benefitCode,
'ราย 6 เดือน' AS paytype,
'20' AS instalment,
savAmt*6 as instalmentAmt,
convert(int,discountSemiAnnual) as discount,
(savAmt*6)- convert(int,discountSemiAnnual) as totInstalmentAmt,
netSemiAnnual as contactAmt,
fundAmt + reward as totContactAmt,
(fundAmt + reward) - netSemiAnnual as netGain from dbo.fundAmt  

union

select age,gender,savAmt,benefitCode,
'ราย 12 เดือน' AS paytype,
'10' AS instalment,
savAmt*10 as instalmentAmt,
convert(int,discountAnnual) as discount,
(savAmt*10)-convert(int,discountAnnual) as totInstalmentAmt,
netAnnual as contactAmt,
fundAmt + reward as totContactAmt,
(fundAmt + reward) - netAnnual as netGain from dbo.fundAmt ) a


   
where gender = '01' and benefitCode = '01' and savAmt = '300' and age = '37' 
 

 select * from dbo.paidUp
 select * from dbo.surrender