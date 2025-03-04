# Small SQL tips.

```sql

-- See Tables in DB: ---------------------------
SELECT TABLE_NAME 
FROM !_YOUR_DATABASE_!.INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'

-- ..


```