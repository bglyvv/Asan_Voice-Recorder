import React, { useState, useEffect } from "react";
import axios from "axios";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlay, faPause, faWifi, faXmark } from "@fortawesome/free-solid-svg-icons";
import { HubConnectionBuilder } from '@microsoft/signalr';
import "./ListComponent.css";

export default function ListComponent() {
  const [devices, setDevices] = useState([]);
  const [connection, setConnection] = useState();
  const [error, setError] = useState();
  const getDevices = () => {
    axios.get("http://localhost:5000/api/device").then((response) => {
      if (response.status === 200) {
        console.log(response.data)
        setDevices(response.data);
      }
    });
  };

  function startDevice (ip)
  {
    console.log(ip)
    connection.invoke("StartConnection",ip).catch(error=>{
      console.log(error)
    })
    
  };

  function startRecording (ip)
  {
    connection.invoke("StartRecord",ip).catch(error=>{
      console.log(error)
    })
    
  };
  const stopDevice = (ip) => 
  {
    console.log(ip)
    connection.invoke("StopConnection",ip).catch(error=>{
      console.log(error)
    })
  };

  const stopRecording = (ip) => 
  {
    connection.invoke("StopRecord",ip).catch(error=>{
      console.log(error)
    })
  };

  useEffect(() => {
    getDevices();
    const newConnection = new HubConnectionBuilder().withUrl("http://localhost:5000/recordhub").build()
    setConnection(newConnection)

  }, []);
  useEffect(() => {
    if (connection) {
        connection.start()
            .then(result => {
                console.log('Connected!');

                connection.on('getChanges', data => {
                    setDevices(data.result)
                });
                connection.on('appError', data =>{
                  alert(data)
                  return
                })
                connection.on('getError', data => {
                  alert(data)
                  return
              });
            })
            .catch(e => console.log('Connection failed: ', e));
    }
}, [connection]);

  return (
    <div className="row tableRow">
      <div className="col-lg-9">
        <table className="table table-dark">
          <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">IP</th>
              <th scope="col">Application Opened</th>
              <th scope="col">Connection Status</th>
              <th scope="col">Connect/Disconnect</th>
              <th scope="col">Recording Status</th>
              <th scope="col">Play/Pause</th>
            </tr>
          </thead>
          <tbody>
            {devices.map((device, id) => {
              return (
                <tr key={id}>
                  <th scope="row">{id + 1}</th>
                  <td>{device.ip}</td>
                  <td>{device.applicationOpened.toString()}</td>
                  <td>{device.connected.toString()}</td>
                  <td>
                    <FontAwesomeIcon
                      style={{ cursor: "pointer" }}
                      icon={faWifi}
                      onClick={() => {startDevice(device.ip)}}
                    />
                    <FontAwesomeIcon
                      style={{ cursor: "pointer" }}
                      icon={faXmark}
                      onClick={() => {stopDevice(device.ip)}}
                    />
                  </td>
                  <td>{device.recording.toString()}</td>
                  <td>
                    <FontAwesomeIcon
                      style={{ cursor: "pointer" }}
                      icon={faPlay}
                      onClick={() => {startRecording(device.ip)}}
                    />
                    <FontAwesomeIcon
                      style={{ cursor: "pointer" }}
                      icon={faPause}
                      onClick={() => {stopRecording(device.ip)}}
                    />
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>

    // <TableContainer>
    //   <Table stickyHeader aria-label="sticky table">
    //     <TableHead>
    //       <TableRow>
    //         <TableCell>#</TableCell>
    //         <TableCell>ID</TableCell>
    //         <TableCell>IP</TableCell>
    //         <TableCell>Connection Status</TableCell>
    //         <TableCell>Play/Pause</TableCell>
    //       </TableRow>
    //     </TableHead>
    //     <TableBody>
    //       {devices.map((device, id) => {
    //         return (
    //           <TableRow hover role="checkbox" tabIndex={-1} key={id}>
    //             <TableCell>{id + 1}</TableCell>
    //             <TableCell>{device.id}</TableCell>
    //             <TableCell>{device.ip}</TableCell>
    //             <TableCell>{device.connected.toString()}</TableCell>
    //             <TableCell>
    //               <FontAwesomeIcon icon={faPlay} />
    //             </TableCell>
    //           </TableRow>
    //         );
    //       })}
    //     </TableBody>
    //   </Table>
    // </TableContainer>
  );
}
