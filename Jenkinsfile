    node('SRVU03') {

      stage('Git checkout') {
          checkout scm
      }
      stage('Deploy run') {
        withCredentials([
          usernamePassword(credentialsId: 'docker_name-minio-cfa', usernameVariable: 'DOCKER_MINIO_USER', passwordVariable: 'DOCKER_MINIO_PASSWORD'),
          usernamePassword(credentialsId: 'catalogServerUserPassword', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD'),
        ]) {
          sh "./jenkins-deploy.sh"
        }
      }
      stage('Clean up our workspace') {
          deleteDir()
          dir("${workspace}@tmp") {
            deleteDir()
          }
      }
    }
