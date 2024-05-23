function copyToClipboard(text) {
    return new Promise((resolve, reject) => {
      navigator.permissions.query({name: "clipboard-write"}).then(result => {
          if (result.state === "granted" || result.state === "prompt") {
              // Clipboard access is granted. Use it to write to the clipboard
              try {
                  navigator.clipboard.writeText(text);
                  console.log('Text copied to clipboard');
                  resolve({'message': `<b>${text}</b> copied to clipboard`, 'type': 'info'});
              } catch (err) {
                  console.log('Failed to copy text: ', err);
                  reject({'message': `Failed to copy text: ${err}`, 'type': 'error'});
              }
          } else {
              console.log('Clipboard access denied');
              reject({'message': 'Clipboard access denied', 'type': 'error'});
          }
      }).catch(err => {
          console.log('Clipboard access denied: ', err);
          reject({'message': `Clipboard access denied: ${err}`, 'type': 'error'});
      });
    });
  }
