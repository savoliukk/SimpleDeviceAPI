pipeline {
  agent any

  stages {
    stage('SSH deploy key check') {
      steps {
        sshagent(credentials: ['github-deploy-key']) {
          sh '''
            set -euxo pipefail
            mkdir -p ~/.ssh
            chmod 700 ~/.ssh
            ssh-keyscan -H github.com >> ~/.ssh/known_hosts
            chmod 600 ~/.ssh/known_hosts

            ssh -T git@github.com || true
          '''
        }
      }
    }
  }
}
