

* https://nicosp.github.io/linux/2021/05/08/pgadmin4-debian.html

```bash
echo "deb https://ftp.postgresql.org/pub/pgadmin/pgadmin4/apt/$(lsb_release -cs) pgadmin4 main" >/etc/apt/sources.list.d/pgadmin4.list
curl https://www.pgadmin.org/static/packages_pgadmin_org.pub | gpg --dearmor > packages.pgadmin.gpg 
install -o root -g root -m 644 packages.pgadmin.gpg /etc/apt/trusted.gpg.d/
rm packages.pgadmin.gpg
apt update
apt install uwsgi-plugin-python3 pgadmin4-server
adduser --system --home /var/lib/pgadmin --disabled-login --shell /usr/sbin/nologin pgadmin4
mkdir /var/log/pgadmin
chown pgadmin4 /var/log/pgadmin
```