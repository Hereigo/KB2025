# Modify String with delimiters:

```sql
DECLARE @lCode CHAR(6) = '123456'

SELECT LEFT(@lCode) + '-' + RIGHT(LEFT(@lCode, 4), 2) + '-' + RIGHT(@lCode, 2)

--------------- output : ---------------
-- 12-34-56

```