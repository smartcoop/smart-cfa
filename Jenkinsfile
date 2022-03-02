    node('SRVU03') {
      stage('Git checkout') {
          checkout scm
      }
      stage('Deploy build') {
        if (['feature/develop'].contains(env.BRANCH_NAME) ) {
          sh "./jenkins-build.sh"
        }
      }
      stage('Deploy run') {
        if (['feature/develop'].contains(env.BRANCH_NAME) ) {
          sh "./jenkins-run.sh"
        }
      }
      stage('Clean up our workspace') {
          deleteDir()
          dir("${workspace}@tmp") {
            deleteDir()
          }
      }
    }
