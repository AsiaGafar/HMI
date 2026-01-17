#!/bin/bash
# File: user-data-monolithic.sh

# Update system
yum update -y

# Install Docker
yum install -y docker
systemctl start docker
systemctl enable docker
usermod -a -G docker ec2-user

# Install Git and build tools
yum install -y git

# Clone and build monolithic version
cd /home/ec2-user
git clone https://github.com/MUnique/OpenMU.git
cd OpenMU

# Copy monolithic configuration files
wget -O Dockerfile.monolithic https://raw.githubusercontent.com/your-repo/openmu-monolithic/main/Dockerfile.monolithic
wget -O appsettings.monolithic.json https://raw.githubusercontent.com/your-repo/openmu-monolithic/main/appsettings.monolithic.json
wget -O startup-monolithic.sh https://raw.githubusercontent.com/your-repo/openmu-monolithic/main/startup-monolithic.sh

# Make startup script executable
chmod +x startup-monolithic.sh

# Build monolithic image
docker build -f Dockerfile.monolithic -t openmu-monolithic:latest .

# Run monolithic container
docker run -d \
  --name openmu-mono \
  --restart unless-stopped \
  -p 80:80 \
  -p 44405:44405 \
  -p 44406:44406 \
  -p 55901:55901 \
  -p 55902:55902 \
  -p 55903:55903 \
  -p 55904:55904 \
  -p 55905:55905 \
  -p 55906:55906 \
  -p 55980:55980 \
  -v /home/ec2-user/openmu-data:/app/data \
  openmu-monolithic:latest

# Create systemd service
cat > /etc/systemd/system/openmu-monolithic.service << EOF
[Unit]
Description=OpenMU Monolithic Server
Requires=docker.service
After=docker.service

[Service]
Type=oneshot
RemainAfterExit=yes
ExecStart=/usr/bin/docker start openmu-mono
ExecStop=/usr/bin/docker stop openmu-mono
User=root

[Install]
WantedBy=multi-user.target
EOF

systemctl enable openmu-monolithic.service
chown -R ec2-user:ec2-user /home/ec2-user