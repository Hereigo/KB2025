### E-mail notification of low disk space.

```bash
#!/bin/bash
used=
df -hl / | awk {'print $4'} | grep "%"
used=${used/\%/}
if [ $used -gt 95 ];
then
echo "Warning! Less than 5% free space. Used = $used"% | mail -s "DISK SPACE WARNING" <a href="mailto:user@host.com">user@host.com</a>
fi
```