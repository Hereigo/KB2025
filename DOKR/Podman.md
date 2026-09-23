```sh

podman machine list
# If needed
podman machine start

# To see containers
podman ps -a
# If needed
podman start [container_name]

# To create persistent container
podman run --name [your-persist-container] --restart=always
# or
podman run --name [your-persist-container] --restart=unless-stopped

# See ENV config
podman exec -it [container_name] printenv
podman exec -it [container_name] printenv | grep ConnectionStrings

```