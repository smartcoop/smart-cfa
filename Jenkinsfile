    node('SRVU03') {
      stage('Git checkout') {
          checkout scm
      }
      stage('Deploy run') {
          sh "./jenkins-deploy.sh"
      }
      stage('Clean up our workspace') {
          deleteDir()
          dir("${workspace}@tmp") {
            deleteDir()
          }
      }
    }
