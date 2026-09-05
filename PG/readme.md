## PostgreSQL

```cmd
net stop postgresql-x64-16
```

```pgsql
CREATE DATABASE db1
    WITH
    OWNER = test_db_admin
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
```
